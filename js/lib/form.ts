export function isTooShort(text: string): boolean{
    return countWords(text) < 3;
}

function countWords(text: string): number {
    if (!text || text.length <= 0) return 0;

    let numWords: number = 1;

    for (let i = 0; i < text.length - 1; i++) {
        const isSpace: boolean = (text[i] == " ");
        const isFollowedByLetter: boolean = charIsLetter(text[i + 1]);

        if (isSpace && isFollowedByLetter) {
            numWords += 1;
        }
    }

    return numWords;
}

function charIsLetter(c: string | undefined): boolean {
    return c != undefined && c.match(/[a-z]/i) != null;
}