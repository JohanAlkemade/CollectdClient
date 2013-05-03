var bla = {
    "EnabledPlugins": [
	    "LogFile", "Redis"
    ],

    "Plugins":
    {
        "LogFile": {
            "LogLevel": "INFO",
            "File": "c:\out.txt",
            "TimeStamp": true
        },
        "WriteGraphite": {
            "Carbon": {
                "Host": "localhost",
                "Port": 2003,
                "Prefix": "collectd"
            }
        },
        "Redis": {
            "redis01": {
                "Host": "redis01.alkmaar.zibernet.eu",
                "Port": 6379,
                "Timeout": 2000
            },
            "redis02": {
                "Host": "redis02.alkmaar.zibernet.eu",
                "Port": 6379,
                "Timeout": 2000
            }
        }
    }
}