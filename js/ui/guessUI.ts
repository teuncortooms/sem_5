import { startGuess, guessCount } from "../lib/guess.js"

const timer = document.getElementById("timer");
const form = document.getElementById("guess-form");
const input = document.getElementById("guess-input");
const output = document.getElementById("guess-output");

export function submitGuess(event: Event): void {
    event.preventDefault();

    if (!(timer instanceof HTMLElement)) {
        console.error("Timer element not found");
        return;
    }

    try {
        const guess = parseGuessedInt();
        const result = startGuess(guess, timer);
        displayResult(result);
    }
    catch (error) {
        console.error("Oh no: " + error);
    }
}


function parseGuessedInt(): number {

    if (!(input instanceof HTMLInputElement)) {
        throw new Error("Invalid HTML element.");
    }

    return parseInt(input.value);
}

function displayResult(result: number): void {
    let message = "";

    if (!output) throw new Error("Output element not found!");
    if (!timer) throw new Error("Timer element not found");

    if (result === -1) message = "Higher!";
    if (result === 1) message = "Lower!";
    if (result === 0) {
        message = "Correct!";
        timer.style.color = "blue";
    }

    output.innerHTML = message + " (" + guessCount + ")";
}

// function displayTime2(element: HTMLElement) {
//     let secString: string = sec.toString();
//     let minString: string = min.toString();
//     let hrString: string = hr.toString();

//     if (sec < 10 || sec === 0) {
//         secString = '0' + sec;
//     }
//     if (min < 10 || min === 0) {
//         minString = '0' + min;
//     }
//     if (hr < 10 || hr === 0) {
//         hrString = '0' + hr;
//     }

//     element.innerHTML = hrString + ':' + minString + ':' + secString;
// }


form?.addEventListener("submit", submitGuess);