#!/bin/bash

results_file="cleaned-python-repositories_$(date +"%Y-%m-%d_%H-%M-%S").csv"
echo "STARTING\n"

for file in python-repositories*.csv; do
    [ -f "$file" ] || break
    echo "File:             $file"
    echo "All lines:        $(wc -l $file)"
    echo "Unique lines      $(sort $file | uniq | wc -l )"
    echo "Writing unique lines to results file..."
    echo
    echo 
    awk '!seen[$0]++' $file >> $results_file
done

echo "Results file:     $results_file"
echo "All lines:        $(wc -l $results_file)"
echo "Unique lines      $(sort $results_file | uniq | wc -l )"
echo "Writing unique lines to results file..."
awk '!seen[$0]++' $results_file > "final-$results_file"

echo "\nFINISHED"