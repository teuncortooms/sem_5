const MIN_GUESS = 0;
const MAX_GUESS = 100;
const number = getNumber();
export let guessCount = 0;
// timer variables
let hr = 0;
let min = 0;
let sec = 0;
let stoptime = true;
function getNumber() {
    return Math.floor(Math.random() * MAX_GUESS) + MIN_GUESS;
}
export function startGuess(guess, displayTimeCallback) {
    if (isNaN(guess))
        throw new Error("Not a number");
    if (guess < MIN_GUESS)
        throw new Error("Number too low.");
    if (guess > MAX_GUESS)
        throw new Error("Number too high.");
    runTimer(displayTimeCallback);
    guessCount++;
    if (guess < number)
        return -1;
    if (guess > number)
        return 1;
    // stop when correct
    stopTimer();
    return 0;
}
function runTimer(displayTimeCallback) {
    if (stoptime === true) {
        resetTimer();
        stoptime = false;
        timerCycle(displayTimeCallback);
    }
}
function resetTimer() {
    hr = min = sec = 0;
}
function stopTimer() {
    if (stoptime === false) {
        stoptime = true;
    }
}
function timerCycle(displayTimeCallback) {
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
        displayTimeCallback({ h: hr, m: min, s: sec });
        setTimeout(function () {
            timerCycle(displayTimeCallback);
        }, 1000);
    }
}
