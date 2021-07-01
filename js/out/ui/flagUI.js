import { getRandomColor } from "../lib/flag.js";
const flagTop = document.getElementById("flag-top");
const flagMiddle = document.getElementById("flag-middle");
const flagBottom = document.getElementById("flag-bottom");
export function changeColor(event) {
    if (!(event.target instanceof HTMLElement)) {
        console.error("Invalid element!");
        return;
    }
    event.target.style.backgroundColor = getRandomColor();
}
flagTop === null || flagTop === void 0 ? void 0 : flagTop.addEventListener("click", changeColor);
flagMiddle === null || flagMiddle === void 0 ? void 0 : flagMiddle.addEventListener('click', changeColor);
flagBottom === null || flagBottom === void 0 ? void 0 : flagBottom.addEventListener('click', changeColor);
