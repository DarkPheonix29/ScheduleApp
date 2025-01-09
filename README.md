PS C:\Users\keala> cd downloads/todolist-backend

PS C:\Users\keala\downloads\todolist-backend> docker network ls
error during connect: Get "http://%2F%2F.%2Fpipe%2FdockerDesktopLinuxEngine/v1.47/networks": open //./pipe/dockerDesktopLinuxEngine: The system cannot find the file specified.
PS C:\Users\keala\downloads\todolist-backend> docker network ls
NETWORK ID     NAME      DRIVER    SCOPE
3f761f0f53dd   bridge    bridge    local
32d569b7debd   host      host      local
b4c69b13d867   none      null      local
PS C:\Users\keala\downloads\todolist-backend> docker network create docker_demo_network_staging
b1ccd0d79a8d7df328e2bce54158b94371bb4d7ddd9b13d06f2e89ba95cfdfd5
PS C:\Users\keala\downloads\todolist-backend> docker network rm docker_demo_network_staging
docker_demo_network_staging
PS C:\Users\keala\downloads\todolist-backend> docker network ls
NETWORK ID     NAME      DRIVER    SCOPE
3f761f0f53dd   bridge    bridge    local
32d569b7debd   host      host      local
b4c69b13d867   none      null      local
PS C:\Users\keala\downloads\todolist-backend> docker run --net docker_demo_network_staging --name docker_demo_db_staging -e MYSQL_DATABASE=docker_demo_db -e MYSQL_ROOT_PASSWORD=my-secret-pw-staging -p 3390:3306 -d mysql
Unable to find image 'mysql:latest' locally
latest: Pulling from library/mysql
7030c241d9b8: Download complete
6ad83e89f981: Download complete
0233a63dc5cd: Download complete
5f31e56c9bea: Download complete
a42d733ea779: Download complete
c0fb96d14e5b: Download complete
f1a9f94fc2db: Download complete
d57074c62694: Download complete
f98254a2b688: Download complete
Digest: sha256:2be51594eba5983f47e67ff5cb87d666a223e309c6c64450f30b5c59a788ea40
Status: Downloaded newer image for mysql:latest
e3219a0f59ae97e07c27cb52559bb17da3bdb7984b851cc26432ecff1c998ccf
6fd1af2601dd: Download complete
Digest: sha256:2be51594eba5983f47e67ff5cb87d666a223e309c6c64450f30b5c59a788ea40
Status: Downloaded newer image for mysql:latest
e3219a0f59ae97e07c27cb52559bb17da3bdb7984b851cc26432ecff1c998ccf
docker: Error response from daemon: network docker_demo_network_staging not found.
PS C:\Users\keala\downloads\todolist-backend>
