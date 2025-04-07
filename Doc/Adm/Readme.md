# Prepare and Configure Hardware

## Configure Service

### Create or Update the Configuration

```bash
sudo  nano /etc/systemd/system/clock.service
```

### The Configuration content

```text
[Unit]
Description=Clock
After=network.target

[Service]
Type=simple
User=clock
Group=clock
WorkingDirectory=/opt/clock
ExecStart=/usr/local/bin/dotnet/dotnet /opt/clock/rdc-svc
Restart=always
Environment=ASPNETCORE_ENVIRONMENT=Production
Environment=DOTNET_CLI_HOME=/opt/clock/.dotnet

[Install]
WantedBy=multi-user.target
```

### Reload the Service Manager

```bash
sudo systemctl daemon-reload
```

### Enable the Service Manager

```bash
sudo systemctl enable clock.service
```

### Start the Service Manager

```bash
sudo systemctl start clock.service
```

### Check the Service Status

```bash
sudo systemctl status clock.service
```

### Display the Service Log

```bash
journalctl -u clock.service -n 100
```
