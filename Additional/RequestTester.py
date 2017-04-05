#!/usr/bin/python3
import sys
import copy
import ast
import time
import requests
import os
if len(sys.argv) < 2:
    print("Please provide matrix name!")
    print("Usage: ./script <Matrix name> <OPT:Refresh time>")
    exit()

req = requests.get("http://webledmatrix.azurewebsites.net/clientApi/Register/" + sys.argv[1])

if req.text == '"Refreshed"':
    print("Matrix already registered. Refreshing matrix.")
elif req.text == '"Registered"':
    print("Matrix is not registered. Registering matrix.")

sleept = 5

if len(sys.argv) > 2:
    sleept = int(sys.argv[2])

class Sender:
    name = "" 
    commands = []

Senders = []
import atexit
def ex():  
    print("Unregistering matrix")
    requests.get("http://webledmatrix.azurewebsites.net/clientApi/Unregister/" + sys.argv[1])
atexit.register(ex)

while True:
    req = requests.get("http://webledmatrix.azurewebsites.net/clientApi/Commands/" + sys.argv[1])
    commands = eval(req.text)
    if os.name=="nt":
        os.system("cls")
    else:
        os.system("clear")
    amm = 0
    if len(Senders) > 0:
        print("Top senders:")
        amm = len(Senders)
    if(len(Senders)>5):
        amm = 5
    
    SSort = copy.deepcopy(Senders)
    STemp = copy.deepcopy(Senders)
    for i in range (0, amm):
        biggest = Sender()
        for j in range (0, amm):
            if(len(biggest.commands) <= len(STemp[j].commands)):
                biggest = copy.deepcopy(STemp[j])
                STemp[j].commands = []
        print(str(i+1) + ". " + biggest.name + "\t\t" + str(len(biggest.commands)))
    print("===============================")
    print("Issued commands:")
    for command in commands:                                                                                              
        sdr = command[0:command.index(':')]
        cmd = command[command.index(':')+1:len(command)]
        print("Sender: " + sdr, end=" ")
        print("\t\tCommand: " + cmd)
        occurs = False
        which = 0
        for i in range(0,len(Senders)):
            if Senders[i].name == sdr:
                occurs = True
                which = i
                break
        if occurs:
            Senders[which].commands.append(cmd)
        else:
            x = Sender()
            x.name = sdr
            x.commands.append(cmd)
            Senders.append(x)
            
    time.sleep(sleept)

