# app collects ping responses and sends it to influxDB
- get list of machines(ip address + hostname) and db name from JSON file
- collects time responses in miliseconds from machines
- each ping request is running in separate thread
- ignore single packet loss
- send data to InfluxDB using POST method
