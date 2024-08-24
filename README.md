# RPIDBClock
Raspberry PI Clock integrated with Deutsche Bahn schedule


### Useful CLI commands

#### Find the process of our application
```
ps -ax | grep rdc-svc
```


### Others

#### Connect to the API in browser:
[http://192.168.1.106:5000/](http://192.168.1.106:5000/)

### Configure Raspberry PI for opening HTTP Port

#### Ensure ufw is installed and enabled
```
sudo apt-get install -y ufw
sudo ufw enable
```

#### Check the status of ufw to ensure the port is open:
```
sudo ufw status
```


#### Open port (for instance, 5000) for incoming connections
```
sudo ufw allow 5000
sudo ufw show added
```