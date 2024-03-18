import { Canvas } from './Canvas';
import { Client, Kind, BeginSessionRequest } from './Client';

console.log('Hello!');

const C = new Canvas(document.body, 600, 400);

let x: number = 0,
    y: number = 0,
    left: boolean = false,
    right: boolean = false,
    up: boolean = false,
    down: boolean = false;

C.onInputDown = (key) => {
    if (key === 'arrowleft') { left = true; }
    if (key === 'arrowright') { right = true; }
    if (key === 'arrowup') { up = true; }
    if (key === 'arrowdown') { down = true; }
};
C.onInputUp = (key) => {
    if (key === 'arrowleft') { left = false; }
    if (key === 'arrowright') { right = false; }
    if (key === 'arrowup') { up = false; }
    if (key === 'arrowdown') { down = false; }
};

/**
 * Some stupid game loop that moves the player around.
 */
function stupidGameLoop() {
    const speed: number = 10;
    if (left) { x -= speed; }
    if (right) { x += speed; }
    if (up) { y -= speed; }
    if (down) { y += speed; }
    C.graphics.clearRect(0, 0, C.width, C.height);
    C.graphics.fillStyle = 'red';
    C.graphics.fillRect(x, y, 30, 30);
}

// Run the stupid game loop every 100ms
setInterval(stupidGameLoop, 100);
let client = new Client();

client.Connect("ws://localhost:9876").then(() => {
    client.Send(new BeginSessionRequest("Hello from the browser!"));
})
