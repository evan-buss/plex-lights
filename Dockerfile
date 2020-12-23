FROM tiangolo/uvicorn-gunicorn-fastapi:python3.8
WORKDIR /app
COPY . .
RUN pip install -r requirements.txt