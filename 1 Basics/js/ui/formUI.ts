import { isTooShort } from "../lib/form.js"

const form = document.getElementById("message-form");
const formOutput = document.getElementById("output-message");

export function submitMessage(event: Event): void {
    event.preventDefault();

    let inputs;

    try {
        inputs = getInputs();

        if (isTooShort(inputs.message)) {
            alertTooShort();
            return;
        }

        processInputs(inputs);
    }
    catch (error) {
        console.error("Oh no: " + error);
    }
}

function getInputs(): { name: string, email: string, message: string } {
    const nameEl = document.getElementById("name-input");
    const emailEl = document.getElementById("email-input");
    const messageEl = document.getElementById("message-input");

    // check if input elements
    if (!(nameEl instanceof HTMLInputElement) ||
        !(emailEl instanceof HTMLInputElement) ||
        !(messageEl instanceof HTMLTextAreaElement)) {
        throw new Error("Invalid HTML element.");
    }

    return { name: nameEl.value, email: emailEl.value, message: messageEl.value };
}

function alertTooShort(): void {
    if (!(formOutput instanceof HTMLElement)) {
        throw new Error("Invalid HTML element");
    }

    formOutput.innerHTML = "Not enough words";
}

function processInputs(inputs: { name: string, email: string, message: string }): void {
    const paragraphs = createParagraphs(inputs.name, inputs.email, inputs.message);
    appendParagraphs(paragraphs);
}


function createParagraphs(name: string, email: string, message: string): { nameP: HTMLParagraphElement, emailP: HTMLParagraphElement, messageP: HTMLParagraphElement } {
    const paragraphs = {
        nameP: document.createElement("p"),
        emailP: document.createElement("p"),
        messageP: document.createElement("p")
    }

    paragraphs.nameP.innerHTML = "Name: " + name;
    paragraphs.emailP.innerHTML = "Email: " + email;
    paragraphs.messageP.innerHTML = "Message: " + message;

    return paragraphs;
}

function appendParagraphs(paragraphs: { nameP: HTMLParagraphElement; emailP: HTMLParagraphElement; messageP: HTMLParagraphElement; }): void {
    if (!(formOutput instanceof HTMLElement)) {
        throw new Error("Output element not found");
    }

    formOutput.innerHTML = "";
    formOutput.appendChild(paragraphs.nameP);
    formOutput.appendChild(paragraphs.emailP);
    formOutput.appendChild(paragraphs.messageP);
}

form?.addEventListener("submit", submitMessage);