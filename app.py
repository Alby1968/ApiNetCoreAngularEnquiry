import os
from flask import Flask

app = Flask(__name__)

@app.route("/")
def home():
    return "Hello Render! 🚀"

PORT = int(os.environ.get("PORT", 5000))

if __name__ == "__main__":
    app.run(host="0.0.0.0", port=PORT)
