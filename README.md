# Project from Distributed Systems of High Availability 

## Running using Docker Swarm
(FIXME: copied from fintech, may not be working correctly)
`docker swarm init` (only at first use)

`docker-compose config >docker-compose.processed.yml`

`docker-compose build --parallel`

`docker stack deploy --compose-file=docker-compose.processed.yml rsww`

To stop:

`docker stack rm fintech`

