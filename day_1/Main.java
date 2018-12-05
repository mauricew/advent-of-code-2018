import java.io.IOException;
import java.nio.file.Files;
import java.nio.file.Path;
import java.nio.file.Paths;
import java.util.*;
import java.util.stream.Collectors;
import java.util.stream.IntStream;

public class Main {

    public static void main(String[] args) {
        Path inputFile = Paths.get("input.txt");

        try {
            List<String> lines = Files.readAllLines(inputFile);

            // nice
            IntStream values = lines.stream()
                    .mapToInt(Integer::parseInt);

            System.out.println(String.format("Answer 1: %d", values.sum()));

            // recursive nice
            List<Integer> listVal = lines.stream()
                    .mapToInt(Integer::parseInt).boxed().collect(Collectors.toList());

            System.out.println(String.format("Answer 2: %d", findFirstDupeSum(listVal)));
        }
        catch (IOException e) {
            System.out.println("No file!");
        }
    }

    public static int findFirstDupeSum(List<Integer> values) {
        Integer curSum = 0;
        Set<Integer> sums = new HashSet<>();

        while (true) {
            for (int num: values) {
                curSum += num;
                if (sums.contains(curSum)) {
                    return curSum;
                }
                sums.add(curSum);
            }
        }
    }
}
