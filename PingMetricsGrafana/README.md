# app collects ping request response and send it to influxDB
- get list of machines(ip address + hostname) and db name from JSON file
- collects time responses in miliseconds from machines
- each ping request is running in separate thread
- send data to InfluxDB using POST method
