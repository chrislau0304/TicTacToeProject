# Import required libraries
from ultralytics import YOLO
import cv2
import cvzone
import math
import pickle
import numpy as np
import mouse
import random
from cvzone.FPS import FPS

# Define various configuration parameters
mode = "mouse"  # "circle" "mouse"
webcamId = 3
camWidth, camHeight = 640, 480
camFPS = 60
confidence = 0.70  # confidence to detect the object
yoloModelPath = "ball.pt"
classNames = ['ball']
calibrationFilePath = 'calibration.p'
ballArea = {"min": 1300, "max": 2500}
scale = 3  # ratio of input vs output image, e.g., 1920/640 = 3



showHitImage = False
verbose = False
fullScreen = False
delayFrames = 15  # number of frames to delay after a hit


# Load Calibration file with Warp Coordinates
fileObj = open(calibrationFilePath, 'rb')
points = pickle.load(fileObj)
fileObj.close()

# Create a new image to display the hits
imgOutput = np.zeros((1080, 1920, 3), np.uint8)
imgOutput[:] = 0, 0, 0  # Background color Black

# For Webcam
#cap = cv2.VideoCapture(webcamId)
cap = cv2.VideoCapture(0)
#cap = cv2.VideoCapture('ball.mp4')  # For Video
cap.set(3, camWidth)
cap.set(4, camHeight)
cap.set(cv2.CAP_PROP_FPS, camFPS)  # to set FPS

# Load YOLO Model
# 其他代碼保持不變
model = None  # 全域變量

# Create FPS Reader
fpsReader = FPS(avgCount=30)


# Variables
hitCount = 0  # to add a delay after detecting a hit
listWrongHits = []


def warpImage(imgMain, circles, width, height):
    """Function to warp image to get a bird's eye view."""
    pts1 = np.float32([circles[0], circles[1], circles[2], circles[3]])
    pts2 = np.float32([[0, 0], [width, 0], [0, height], [width, height]])
    matrix = cv2.getPerspectiveTransform(pts1, pts2)
    imgOutput = cv2.warpPerspective(imgMain, matrix, (width, height))
    return imgOutput


def detectObject(imgMain):
    """Function to detect objects in the input image using YOLO."""

    global model
    if model is None:
        model = YOLO(yoloModelPath)

    results = model(imgMain, stream=False, verbose=False)
    objects = []
    for r in results:
        boxes = r.boxes
        for box in boxes:
            x1, y1, x2, y2 = box.xyxy[0]  # Bounding Box
            x1, y1, x2, y2 = int(x1), int(y1), int(x2), int(y2)
            conf = math.ceil((box.conf[0] * 100)) / 100  # Confidence
            cls = int(box.cls[0])  # Class Name
            #if cls < len(classNames) and conf > confidence:  # Check if cls is within bounds
            if conf > confidence:
                area = (x2 - x1) * (y2 - y1)
                center = x1 + (x2 - x1) // 2, y1 + (y2 - y1) // 2

                # Draw the detected object's class name, confidence, and bounding box on the image
                #cvzone.putTextRect(imgMain, f'{classNames[cls]} {conf}',
                cvzone.putTextRect(imgMain, f'{classNames} {conf}',
                                   (max(0, x1), max(35, y1)), scale=1, thickness=1, colorB=[0, 245, 0],
                                   colorT=(255, 255, 255), colorR=[0, 245, 0], offset=5)
                cvzone.putTextRect(imgMain, f'{area}',
                                   (max(0, x1), max(35, y2)), scale=1, thickness=1, colorB=[0, 245, 0],
                                   colorT=(255, 255, 255, 255), colorR=[0, 245, 0], offset=5)
                cv2.rectangle(imgMain, (x1, y1), (x2, y2), (0, 245, 0), 3)
                objects.append([x1, y1, x2, y2, area, center, imgMain])

        return imgMain, objects



def detectHit(objects):
    """Function to detect a hit based on the area of the detected object."""
    for obj in objects:
        x1, y1, x2, y2, area, center, _ = obj
        if ballArea["min"] < area < ballArea["max"]:
            return obj



def drawTarget(imgDraw, targetDic, color=[255, 0, 255]):
    cv2.circle(imgDraw, targetDic["center"], targetDic["radius"], color, cv2.FILLED)
    cv2.circle(imgDraw, targetDic["center"], targetDic["radius"] - 10, (255, 255, 255), 5)
    cv2.circle(imgDraw, targetDic["center"], 20, (255, 255, 255), 3)


def circleToRectangle(center, radius):
    cx, cy = center
    x1 = cx - radius
    y1 = cy - radius
    x2 = cx + radius
    y2 = cy + radius

    return [x1, y1, x2, y2]


def drawHit(imgDraw, obj, type="circle", imgToOverlay=[], showHitImage=False):
    """ types "circle" "mouse" """
    x = int(obj[5][0] * scale)
    y = int(obj[5][1] * scale)
    print(obj[4])


    if type == "mouse":
        # move mouse to position
        mouse.move(x, y)
        # left click
        mouse.click('left')


    elif type == "circle":
        cv2.circle(imgDraw, (x, y), 20, (0, 200, 0), cv2.FILLED)

    if showHitImage: cv2.imshow(f"{obj[4]}", obj[6])

    return imgDraw



# Main loop for processing frames from the webcam and performing hit detection
while True:
    success, img = cap.read()
    imgProjector = warpImage(img, points, 640, 360)
    imgWithObjects, objects = detectObject(imgProjector)

    # Perform hit detection and draw hit if found
    hitPoint = detectHit(objects)

    if hitPoint and hitCount == 0:
        imgOutput = drawHit(imgOutput, hitPoint, type=mode, showHitImage=showHitImage)

        hitPoint = []
        hitCount = 1

    if hitCount != 0:
        hitCount += 1
        if hitCount >= 10:
            hitCount = 0

    # Update FPS and display images
    fps, img = fpsReader.update(imgWithObjects)

    # Display
    cv2.imshow("Image", img)
    #cv2.imshow("Image Projector", imgWithObjects)
    if mode != "mouse":
        if fullScreen:
            cv2.setWindowProperty("Image Out", cv2.WND_PROP_FULLSCREEN, cv2.WINDOW_FULLSCREEN)
        cv2.namedWindow("Image Out", cv2.WINDOW_NORMAL)
        cv2.imshow("Image Out", imgOutput)

    key = cv2.waitKey(1)
    if key == ord('r'):  # press r to reset the image
        imgOutput = np.zeros((1080, 1920, 3), np.uint8)
        imgOutput[:] = 0, 0, 0
    if key == ord('q'):  # press q to quit
        break
