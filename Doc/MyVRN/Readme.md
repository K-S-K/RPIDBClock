# MY VRN API Description

## Main Page URL 1

<https://www.vrn.de/mng/#/XSLT_TRIP_REQUEST2@init?mode=sharing&restriction=0&orig=6002080&dest=6001160&date=28.08.2024&time=09:00&isDeparture=true>

<https://www.vrn.de/mng/#/XSLT_TRIP_REQUEST2@init?mode=sharing&restriction=0&orig=6002080&dest=6001160&date=26.03.2025&time=00:00&isDeparture=true>

XML_TRIP_REQUEST2?ca

## Main Page URL 2

<https://www.vrn.de/mng/#/XSLT_TRIP_REQUEST2@init?mode=sharing&restriction=0&orig=6001160&dest=6002080&date=29.08.2024&time=16:015&isDeparture=true>

## Data Request URL1

<https://www.vrn.de/mngvrn/XML_TRIP_REQUEST2?changeSpeed=normal&coordOutputFormat=EPSG:4326&cycleSpeed=14&deleteITPTWalk=0&exclMOT_15=1&exclMOT_16=1&excludedMeans=checkbox&itOptionsActive=1&itPathListActive=1&itdDate=20240828&itdTime=0900&lineRestriction=0400&locationServerActive=1&name_destination=de:08221:1160&name_origin=de:07314:2080&outputFormat=json&ptMacro=true&ptOptionsActive=1&routeType=leasttime&strictMode=0&trITMOT=100&trITMOTvalue=15&type_destination=any&type_origin=any&useElevationData=1&useRealtime=1&useUT=1&useUnifiedTickets=1&wheelchairSpaceStop=0>

Request Method
GET
Status Code:
200 OK
Remote Address:
145.253.183.38:443
Referrer Policy:
strict-origin-when-cross-origin

Request Headers:

```text
GET /mngvrn/XML_TRIP_REQUEST2?changeSpeed=normal&coordOutputFormat=EPSG:4326&cycleSpeed=14&deleteITPTWalk=0&exclMOT_15=1&exclMOT_16=1&excludedMeans=checkbox&itOptionsActive=1&itPathListActive=1&itdDate=20240828&itdTime=0900&lineRestriction=0400&locationServerActive=1&name_destination=de:08221:1160&name_origin=de:07314:2080&outputFormat=json&ptMacro=true&ptOptionsActive=1&routeType=leasttime&strictMode=0&trITMOT=100&trITMOTvalue=15&type_destination=any&type_origin=any&useElevationData=1&useRealtime=1&useUT=1&useUnifiedTickets=1&wheelchairSpaceStop=0 HTTP/1.1
Accept: application/json, text/plain, */*
Accept-Encoding: gzip, deflate, br, zstd
Accept-Language: en-US,en;q=0.9,de;q=0.8
Connection: keep-alive
Cookie: hidecookie=true; allcookies=true; _pk_id.2.4095=32d19f7db1f38df6.1724792043.; _pk_ses.2.4095=1; _ga=GA1.2.619936216.1724792156; _gid=GA1.2.2035674586.1724792156
Host: www.vrn.de
Referer: https://www.vrn.de/mng/
Sec-Fetch-Dest: empty
Sec-Fetch-Mode: cors
Sec-Fetch-Site: same-origin
User-Agent: Mozilla/5.0 (Macintosh; Intel Mac OS X 10_15_7) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/127.0.0.0 Safari/537.36
sec-ch-ua: "Not)A;Brand";v="99", "Google Chrome";v="127", "Chromium";v="127"
sec-ch-ua-mobile: ?0
sec-ch-ua-platform: "macOS"
```

Responce Headers:

```text
HTTP/1.1 200 OK
Date: Tue, 27 Aug 2024 20:55:56 GMT
Server: EFAController/10.4.30.7/EMA-4
Content-Type: application/json
Accept-Ranges: none
Content-Length: 29237
Content-Encoding: gzip
Keep-Alive: timeout=5, max=99
Connection: Keep-Alive
```

### Data Request URL2

<https://www.vrn.de/mngvrn/XML_TRIP_REQUEST2?changeSpeed=normal&coordOutputFormat=EPSG:4326&cycleSpeed=14&deleteITPTWalk=0&exclMOT_15=1&exclMOT_16=1&excludedMeans=checkbox&itOptionsActive=1&itPathListActive=1&itdDate=20240829&itdTime=1601&lineRestriction=0400&locationServerActive=1&name_destination=de:07314:2080&name_origin=de:08221:1160&outputFormat=json&ptMacro=true&ptOptionsActive=1&routeType=leasttime&strictMode=0&trITMOT=100&trITMOTvalue=15&type_destination=any&type_origin=any&useElevationData=1&useRealtime=1&useUT=1&useUnifiedTickets=1&wheelchairSpaceStop=0>

### CURL

```bash
curl 'https://www.vrn.de/mngvrn/XML_TRIP_REQUEST2?changeSpeed=normal&coordOutputFormat=EPSG:4326&cycleSpeed=14&deleteITPTWalk=0&exclMOT_15=1&exclMOT_16=1&excludedMeans=checkbox&itOptionsActive=1&itPathListActive=1&itdDate=20240829&itdTime=1601&lineRestriction=0400&locationServerActive=1&name_destination=de:07314:2080&name_origin=de:08221:1160&outputFormat=json&ptMacro=true&ptOptionsActive=1&routeType=leasttime&strictMode=0&trITMOT=100&trITMOTvalue=15&type_destination=any&type_origin=any&useElevationData=1&useRealtime=1&useUT=1&useUnifiedTickets=1&wheelchairSpaceStop=0' \
  -H 'Accept: application/json, text/plain, */*' \
  -H 'Accept-Language: en-US,en;q=0.9,de;q=0.8' \
  -H 'Connection: keep-alive' \
  -H 'Cookie: hidecookie=true; allcookies=true; _pk_id.2.4095=32d19f7db1f38df6.1724792043.; _ga=GA1.2.619936216.1724792156; _ga_XD83M7R2HV=GS1.2.1724792155.1.1.1724792155.0.0.0; _gid=GA1.2.1458266347.1724940820; _gat=1' \
  -H 'Referer: https://www.vrn.de/mng/' \
  -H 'Sec-Fetch-Dest: empty' \
  -H 'Sec-Fetch-Mode: cors' \
  -H 'Sec-Fetch-Site: same-origin' \
  -H 'User-Agent: Mozilla/5.0 (Macintosh; Intel Mac OS X 10_15_7) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/127.0.0.0 Safari/537.36' \
  -H 'sec-ch-ua: "Not)A;Brand";v="99", "Google Chrome";v="127", "Chromium";v="127"' \
  -H 'sec-ch-ua-mobile: ?0' \
  -H 'sec-ch-ua-platform: "macOS"'
```
