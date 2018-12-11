# azure-storage-test

Azure storage test based on:

* MSDN [quick start](https://docs.microsoft.com/en-us/azure/storage/blobs/storage-quickstart-blobs-dotnet?tabs=windows)
* Docker container [azure-storage-emulator](https://hub.docker.com/r/microsoft/azure-storage-emulator/)

Before running code, run container:

`docker run -p 10000:10000 -p 10001:10001 -p 10002:10002 microsoft/azure-storage-emulator`
