import { startGuess, guessCount } from "../lib/guess.js";
const timer = document.getElementById("timer");
const form = document.getElementById("guess-form");
const input = document.getElementById("guess-input");
const output = document.getElementById("guess-output");
export function submitGuess(event) {
    event.preventDefault();
    if (!(timer instanceof HTMLElement)) {
        console.error("Timer element not found");
        return;
    }
    try {
        const guess = parseGuessedInt();
        const result = startGuess(guess, displayTime);
        displayResult(result);
    }
    catch (error) {
        console.error("Oh no: " + error);
    }
}
function parseGuessedInt() {
    if (!(input instanceof HTMLInputElement)) {
        throw new Error("Invalid HTML element.");
    }
    return parseInt(input.value);
}
function displayResult(result) {
    let message = "";
    if (!output)
        throw new Error("Output element not found!");
    if (!timer)
        throw new Error("Timer element not found");
    if (result === -1)
        message = "Higher!";
    if (result === 1)
        message = "Lower!";
    if (result === 0) {
        message = "Correct!";
        timer.style.color = "blue";
    }
    output.innerHTML = message + " (" + guessCount + ")";
}
function displayTime(time) {
    let secString = time.s.toString();
    let minString = time.m.toString();
    let hrString = time.h.toString();
    if (!timer)
        throw new Error("No timer element!");
    if (time.s < 10 || time.s === 0) {
        secString = '0' + time.s;
    }
    if (time.m < 10 || time.m === 0) {
        minString = '0' + time.m;
    }
    if (time.h < 10 || time.h === 0) {
        hrString = '0' + time.h;
    }
    timer.innerHTML = hrString + ':' + minString + ':' + secString;
}
form === null || form === void 0 ? void 0 : form.addEventListener("submit", submitGuess);
