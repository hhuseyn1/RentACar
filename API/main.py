import sys
import subprocess
import pkg_resources
from math import trunc

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
# 'http://127.0.0.1:8000/DeleteUser?Username=Salam&Password=Adam'

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

FetchData()

@app.get("/")
def read_root():
    return {"Hello": "World"}

@app.get("/Register")
def Register(Firstname: str, Lastname: str, Email: str, Username: str, Password: str):
    try:
        Cursor.execute(f"INSERT INTO users (Username, Password, Name, Lastname, Email) VALUES ('{Username}', '{Password}', '{Firstname}', '{Lastname}', '{Email}')")
        Connection.commit()
        return "Succesfully registered"
    except Exception:
        return "Same Username"

@app.get("/GetUser")
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
        try:
            Cursor.execute(f"SELECT * FROM users WHERE Username = '{Username}' AND Password = '{Password}'")
            Connection.commit()

            LastUser = Cursor.fetchall()[0]
            return LastUser
        except Exception:
            return []
    except Exception:
        return []

@app.get("/AddUser")
def AddVehicle(Username: str, Password: str, Name: str, Lastname: str, Email: str, IsAdmin: bool):
    try:
        Cursor.execute(f"INSERT INTO users (Username, Password, Name, Lastname, Email, IsAdmin) VALUES ('{Username}', '{Password}', '{Name}', '{Lastname}', '{Email}', {IsAdmin})")
        Connection.commit()
    except Exception:
        return "Username Exist!"
    FetchData()
    return "Succesfully Added"

@app.get("/DeleteUser")
def DeleteUser(Username: str):
    try:
        Cursor.execute(f"DELETE FROM users WHERE Username = '{Username}'")
        Connection.commit()
    except Exception:
        print("No Result")
    return

@app.get("/AddVehicle")
def AddVehicle(Make: str, Model: str, Plate: str, Price: str):
    Cursor.execute(f"INSERT INTO cars (Make, Model, Plate, Price, Page) VALUES ('{Make}', '{Model}', '{Plate}', '{Price}', '{trunc(len(CarData)/10) + 1}')")
    Connection.commit()
    FetchData()
    return "Succesfully Added"

@app.get("/DeleteVehicle")
def DeleteVehicle(Id: int):
    try:
        Cursor.execute(f"DELETE FROM cars WHERE Id = '{Id}'")
        Connection.commit()
    except Exception:
        print("No Result")
    return

@app.get("/GetCars")
def GetCars():
    global CarData
    return CarData

@app.get("/GetUsers")
def GetCars():
    global UserData
    return UserData 

@app.get("/Test")
def GetCars():
    global UserData
    return len(CarData)

if __name__ == "__main__":
    uvicorn.run(app, host="0.0.0.0", port=8000)