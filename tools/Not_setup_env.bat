set "PROJECT_DIR=%~dp0"

cd /d "%PROJECT_DIR%"

pip install virtualenv

virtualenv .venv

pause
