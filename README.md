# hospital-managment-api
## Table of contents
* [General info](#general-info)
* [Technologies](#technologies)
* [Setup](#setup)

## General info
This project is a web api for hospital managment system.
	
## Technologies
Project is created with:
* Asp net core 6.0
* Docker
* PostgreSQL

## Setup
To run this project, install it locally:
Clone the repository
open hospital-managment-api.sln in Visual Studio 2022 and run
or use Docker from cmd.
```
$ cd hospital-managment-api
$ docker build -f hospital-managment-api/Dockerfile -t hospital-managment-api:dev .
$ docker run --name hospital-managment-api:dev -p 8081:80 -d hospital-managment-api:dev
```
Api should be ready to use