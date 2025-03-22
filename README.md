# RPIDBClock

Raspberry PI Clock integrated with Deutsche Bahn schedule

## Useful CLI commands

### Build the whole solution

``` bash
dotnet build Src --no-incremental
```

### Execute all tests

``` bash
dotnet test Src --no-build
```

### Connect to the RPI

``` bash
ssh 192.168.1.106
```

The password is 12434

### Find the process of our application

``` bash
ps -ax | grep rdc-svc
```

## Others

### Connect to the API in browser

[http://192.168.1.106:5000/](http://192.168.1.106:5000/)

### Configure Raspberry PI for opening HTTP Port

#### Ensure ufw is installed and enabled

``` bash
sudo apt-get install -y ufw
sudo ufw enable
```

#### Check the status of ufw to ensure the port is open

``` bash
sudo ufw status
```

#### Open port (for instance, 5000) for incoming connections

``` bash
sudo ufw allow 5000
sudo ufw show added
```
