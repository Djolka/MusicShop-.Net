export class Filter {
    acousticguitar: number = 0;
    electricguitar: number = 0;
    bassguitar: number = 0;
    soprano: number = 0;
    concert: number = 0;
    tenor: number = 0;
    acousticdrums: number = 0;
    electronicdrums: number = 0;
    piano: number = 0;
    synthesizer: number = 0;
    acousticamp: number = 0;
    electricamp: number = 0;
    bassamp: number = 0;
    microphone: number = 0;
    strings: number = 0;
    pick: number = 0;
    capo: number = 0;

    public changeValue(id: keyof Filter, value: any) {
        this[id] = value
    }

    public areAllUnchecked(){
        for (const key in this) {
            if (this[key] !== 0) {
                return false;
            }
        }
        return true;
    }
}