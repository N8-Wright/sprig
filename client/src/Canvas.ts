export class Canvas {
    public readonly canvas: HTMLCanvasElement = document.createElement('canvas');
    public readonly graphics: CanvasRenderingContext2D = this.canvas.getContext('2d')!;
    public onInputDown?: (key: string) => void;
    public onInputUp?: (key: string) => void;
    constructor(parent: HTMLElement, public readonly width: number, public readonly height: number) {
        this.canvas.style.width = width.toFixed() + 'px';
        this.canvas.style.height = height.toFixed() + 'px';
        this.canvas.width = width;
        this.canvas.height = height;
        this.canvas.tabIndex = 0;
        this.canvas.addEventListener('keydown', e => this.onInputDown && this.onInputDown(e.key.toLowerCase()));
        this.canvas.addEventListener('keyup', e => this.onInputUp && this.onInputUp(e.key.toLowerCase()));
        this.canvas.addEventListener('contextmenu', e => e.preventDefault());
        parent.appendChild(this.canvas);
        this.canvas.focus();
    }
}