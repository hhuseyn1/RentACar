import sys
import subprocess
import pkg_resources

required = {'uvicorn', 'mysql.connector', 'fastapi'}
installed = {pkg.key for pkg in pkg_resources.working_set}
missing = required - installed

if missing:
    python = sys.executable
    subprocess.check_call([python, '-m', 'pip', 'install', *missing], stdout=subprocess.DEVNULL)

import uvicorn
import threading
import mysql.connector
from fastapi import FastAPI

app = FastAPI()
CarData = {}
UserData = {}
LastUser = {}

# uvicorn RentACar:app --reload

DbConnection = {
    "Host": "176.53.69.151",
    "Database": "unexfive_daily",
    "User": "unexfive_Daily",
    "Password": "0e1*37cnB",
}

Connection = mysql.connector.connect(host=DbConnection["Host"], database=DbConnection["Database"], user=DbConnection["User"], password=DbConnection["Password"])
Cursor = Connection.cursor(buffered=True, dictionary=True)
print("Database Connection Established")

def FetchData():
    global CarData
    global UserData
    Cursor.execute(f"SELECT * FROM cars")
    Connection.commit()
    CarData = Cursor.fetchall()
    Cursor.execute(f"SELECT * FROM users")
    Connection.commit()
    UserData = Cursor.fetchall()
    threading.Timer(3.0, FetchData).start()
    print("Data Fetched")

FetchData()

@app.get("/")
def read_root():
    return {"Hello": "World"}

@app.get("/GetUser")
# 'http://127.0.0.1:8000/GetUser?Username=Salam&Password=Adam'
def GetUser(Username: str, Password: str):
    global LastUser
    try:
        if LastUser["Username"] == Username and LastUser["Password"] == Password:
            return LastUser
        else:
            Cursor.execute(f"SELECT * FROM users WHERE Username = '{Username}' AND Password = '{Password}'")
            Connection.commit()

            LastUser = Cursor.fetchall()[0]
            return LastUser
    except KeyError:
        Cursor.execute(f"SELECT * FROM users WHERE Username = '{Username}' AND Password = '{Password}'")
        Connection.commit()

        LastUser = Cursor.fetchall()[0]
        return LastUser
    except Exception:
        return []
        

@app.get("/GetCars")
def GetCars():
    global CarData
    return CarData

@app.get("/GetUsers")
def GetCars():
    global UserData
    return UserData 

if __name__ == "__main__":
    uvicorn.run(app, host="0.0.0.0", port=8000)