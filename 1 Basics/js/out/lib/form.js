export function isTooShort(text) {
    return countWords(text) < 3;
}
function countWords(text) {
    if (!text || text.length <= 0)
        return 0;
    let numWords = 1;
    for (let i = 0; i < text.length - 1; i++) {
        const isSpace = (text[i] == " ");
        const isFollowedByLetter = charIsLetter(text[i + 1]);
        if (isSpace && isFollowedByLetter) {
            numWords += 1;
        }
    }
    return numWords;
}
function charIsLetter(c) {
    return c != undefined && c.match(/[a-z]/i) != null;
}
