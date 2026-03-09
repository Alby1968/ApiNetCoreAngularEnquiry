# Usa immagine base Python
FROM python:3.11-slim

# Imposta la directory di lavoro
WORKDIR /app

# Copia il file delle dipendenze e installa
COPY requirements.txt .
RUN pip install --no-cache-dir -r requirements.txt

# Copia tutto il progetto
COPY . .

# Espone la porta su cui gira l'app
EXPOSE 5000

# Comando per avviare l'app
CMD ["python", "app.py"]
