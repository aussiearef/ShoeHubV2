# Observability with Grafana, Prometheus and Loki
## The ShoeHub  (Prometheus Exporter)

[![Grafana, Graphite, and StatsD](https://img-c.udemycdn.com/course/750x422/1473698_386a_9.jpg)](https://www.udemy.com/course/grafana-graphite-and-statsd-visualize-metrics/?referralCode=F9360D03CB430529BEAD)

This repository belongs to an online course called **"Observability with Grafana, Prometheus and Loki"**. The code generates random data points for an imaginary company called ShoeHub. 

To access the metrics, run the application using the executable file provided under the **Releases** folder relevant to your operating system (i.e., ShoeHubV2.exe in Windows). If you prefer to use the provided Docker container, follow the instructions below.

The application includes a built-in web server, so once the application is run, you will see the address where you can see the metrics under __/metrics__ endpoint.


[![Example of where to find the host address and port](https://github.com/aussiearef/ShoeHubV2/blob/main/host-example.png?raw=true)](https://github.com/aussiearef/ShoeHubV2/blob/main/host-example.png?raw=true)

In the above example, you must scrape HTTP://locahost:5000/metrics to see the metrics.

Use the prometheus.yml file that is provided in the repository. It contains a scrape configuration that you can use in Prometheus:

```yaml
scrape_configs:
  - job_name: 'shoehub'
    scrape_interval: 5s
    static_configs:
      - targets: ['http://localhost:80']
```

### Running the Docker Container

If you have Docker Desktop running, pull the provided Docker image:

```
  docker pull aussiearef/shoehub
```

By default, the metric endpoint listens on port __8080__. If you want to assign a different port to the metrics endpoint, use the -p argument when running the __docker run__ command:

```
  docker run -p <local port>:8080 -i aussiearef/shoehub
```

Example:

```
  docker run -p 8020:80 -i aussiearef/shoehub
```

Please remember to update your scraping rule in Prometheus so it scrapes the correct port.

### Building the code

The code is written in .NET 8 as a minimal web API. If you wish to build the code, [download the .NET 8 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
.NET 8 is cross-platform so you can download and install it on Windows, Mac and Linux.

Then, clone the code, and in Terminal (or Command Prompt), navigate to the **ShoeHubV2** folder. Then run **dotnet run**


