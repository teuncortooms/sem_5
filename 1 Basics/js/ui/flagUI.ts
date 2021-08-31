import { getRandomColor } from "../lib/flag.js"

const flagTop = document.getElementById("flag-top");
const flagMiddle = document.getElementById("flag-middle");
const flagBottom = document.getElementById("flag-bottom");

export function changeColor(event: Event): void {
    if (!(event.target instanceof HTMLElement)) {
        console.error("Invalid element!");
        return;
    }

    event.target.style.backgroundColor = getRandomColor();

}

flagTop?.addEventListener("click", changeColor);
flagMiddle?.addEventListener('click', changeColor);
flagBottom?.addEventListener('click', changeColor);