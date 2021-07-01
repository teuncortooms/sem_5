const MIN_GUESS: number = 0;
const MAX_GUESS: number = 100;

const number: number = getNumber();
export let guessCount: number = 0;

// timer variables
let hr: number = 0;
let min: number = 0;
let sec: number = 0;
let stoptime: boolean = true;

function getNumber(): number {
    return Math.floor(Math.random() * MAX_GUESS) + MIN_GUESS;
}

export function startGuess(guess: number, element: HTMLElement): number {
    if (isNaN(guess)) throw new Error("Not a number");
    if (guess < MIN_GUESS) throw new Error("Number too low.");
    if (guess > MAX_GUESS) throw new Error("Number too high.");

    startTimer(element);
    guessCount++;

    if (guess < number) return -1;
    if (guess > number) return 1;

    // stop when correct
    stopTimer();
    return 0;
}

function startTimer(element: HTMLElement): void {
    if (stoptime === true) {
        stoptime = false;
        timerCycle(element);
    }
}

function stopTimer(): void {
    if (stoptime === false) {
        stoptime = true;
    }
}

function timerCycle(element: HTMLElement): void {
    if (stoptime === false) {
        sec = sec + 1;

        if (sec === 60) {
            min = min + 1;
            sec = 0;
        }
        if (min === 60) {
            hr = hr + 1;
            min = 0;
            sec = 0;
        }

        displayTime(element);

        setTimeout("timerCycle()", 1000);
    }

    // should not be part of this lib --> Angular will help there
    function displayTime(element: HTMLElement) {
        let secString: string = sec.toString();
        let minString: string = min.toString();
        let hrString: string = hr.toString();

        if (sec < 10 || sec === 0) {
            secString = '0' + sec;
        }
        if (min < 10 || min === 0) {
            minString = '0' + min;
        }
        if (hr < 10 || hr === 0) {
            hrString = '0' + hr;
        }

        element.innerHTML = hrString + ':' + minString + ':' + secString;
    }
}

