import cv2
import numpy as np
import pickle
#import time

width, height = 640, 360
#cap = cv2.VideoCapture(3)  # For Webcam
cap = cv2.VideoCapture(0)
#cap = cv2.VideoCapture('ball.mp4')  # For Video

fileName = 'calibration.p'

cap.set(3, 640)
cap.set(4, 480)
circles = np.zeros((4, 2), int)
counter = 0

# 取得影片的幀率
fps = cap.get(cv2.CAP_PROP_FPS)
frame_duration = 1 / fps

def mousePoints(event, x, y, flags, params):
    global counter
    if event == cv2.EVENT_LBUTTONDOWN:
        circles[counter] = x, y
        counter += 1
        print(circles)

while True:
    #start_time = time.time()  # 記錄開始時間
    success, img = cap.read()

    if not success:
        break

    if counter == 4:
        fileObj = open(fileName, 'wb')
        pickle.dump(circles, fileObj)
        fileObj.close()

        pts1 = np.float32([circles[0], circles[1], circles[2], circles[3]])
        pts2 = np.float32([[0, 0], [width, 0], [0, height], [width, height]])
        matrix = cv2.getPerspectiveTransform(pts1, pts2)
        imgOutput = cv2.warpPerspective(img, matrix, (width, height))
        cv2.imshow("Output Image ", imgOutput)

    for x in range(0, 4):
        cv2.circle(img, (circles[x][0], circles[x][1]), 3, (0, 255, 0), cv2.FILLED)

    cv2.imshow("Original Image ", img)
    cv2.setMouseCallback("Original Image ", mousePoints)

    # 計算處理當前幀所需的時間
    #end_time = time.time()
    #process_time = end_time - start_time

    # 如果處理時間小於幀長度，則延遲剩餘的時間
    #if process_time < frame_duration:
        #time.sleep(frame_duration - process_time)

    if cv2.waitKey(1) & 0xFF == ord('q'):
        break

cap.release()
cv2.destroyAllWindows()
