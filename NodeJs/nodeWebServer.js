const http = require('http');

const requestListener = (req, response) => {
    //response.end('Hellow world');
    response.end('Check web server console');
    console.log(req.body);
}

const server = http.createServer();
server.on('request', requestListener);

server.listen(4242, () => {
 console.log('Server is running...')
});