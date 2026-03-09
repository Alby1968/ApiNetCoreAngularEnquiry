# Usa immagine base Python leggera
FROM python:3.11-slim

# Imposta la directory di lavoro dentro il container
WORKDIR /app

# Copia requirements.txt se esiste e installa dipendenze
COPY requirements.txt .
RUN pip install --no-cache-dir -r requirements.txt || echo "No requirements.txt found, skipping install"

# Copia tutto il progetto nella directory di lavoro
COPY . .

# Espone la porta che Render utilizzerà
EXPOSE 5000

# Comando per avviare l'app
CMD ["python", "app.py"]
