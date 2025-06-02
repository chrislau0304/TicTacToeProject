powershell -Command "Invoke-WebRequest -Uri 'https://www.python.org/ftp/python/3.11.6/python-3.11.6-amd64.exe' -OutFile 'python-3110.exe'"

start /wait python-3110.exe /quiet InstallAllUsers=1 PrependPath=1

del python-3110.exe

pause
