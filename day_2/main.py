def get_line_dupes(line):
    char_map = {}
    
    for char in line:
        char_count = char_map.get(char, 0)
        char_map[char] = char_count + 1

    twos_dupes = False
    threes_dupes = False
        
    for char in char_map.keys():
        if char_map[char] == 2:
            twos_dupes = True
        if char_map[char] == 3:
            threes_dupes = True

    return twos_dupes, threes_dupes

def get_checksum(lines):
    twos_count = 0
    threes_count = 0
    for line in lines:
        has_twos, has_threes = get_line_dupes(line)
        if has_twos:
            twos_count = twos_count + 1
        if has_threes:
            threes_count = threes_count + 1

    return twos_count * threes_count

def get_shared_id(lines):
    cur_word = None
    prev_word = None
    
    for line_num in range(len(lines)):
        cur_word = lines[line_num]

        for other_line_num in range(len(lines)):
            if line_num == other_line_num:
                continue

            intersection = words_similar(cur_word, lines[other_line_num])
            if intersection is not None:
                return intersection
                
def words_similar(first_word, second_word):
    diff_count = 0
    best_word_one = None
    best_word_two = None
    intersection = []
    
    for i in range(len(first_word)):
        if first_word[i] != second_word[i]:
            diff_count = diff_count + 1
        else:
            intersection.append(first_word[i])
    if diff_count != 1:
        return None
    return "".join(intersection)

def main():
    with open("input.txt") as inputFile:
        lines = inputFile.readlines()
    lines = [line.strip() for line in lines]

    checksum = get_checksum(lines)
    print(checksum)

    shared_id = get_shared_id(lines)
    print(shared_id)
        
    
if __name__ == "__main__":
    main()
