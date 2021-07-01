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

interface IDisplayTimerFunction {
    (time: { h: number, m: number, s: number }): void;
}

export function startGuess(guess: number, displayTimeCallback: IDisplayTimerFunction): number {
    if (isNaN(guess)) throw new Error("Not a number");
    if (guess < MIN_GUESS) throw new Error("Number too low.");
    if (guess > MAX_GUESS) throw new Error("Number too high.");

    runTimer(displayTimeCallback);
    guessCount++;

    if (guess < number) return -1;
    if (guess > number) return 1;

    // stop when correct
    stopTimer();
    return 0;
}

function runTimer(displayTimeCallback: IDisplayTimerFunction): void {
    if (stoptime === true) {
        resetTimer();
        stoptime = false;
        timerCycle(displayTimeCallback);
    }
}

function resetTimer() {
    hr = min = sec = 0;
}

function stopTimer(): void {
    if (stoptime === false) {
        stoptime = true;
    }
}

function timerCycle(displayTimeCallback: IDisplayTimerFunction): void {
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

        displayTimeCallback({h: hr, m: min, s: sec});

        // doubting whether this event should go in the ui
        setTimeout(function(){
            timerCycle(displayTimeCallback);
        }, 1000);
    }
}



