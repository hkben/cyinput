#Filter the converted file for duplication
import codecs
infile = open ('output.dll',encoding="utf-8")
outfile = codecs.open("mapped_homograph.dll", "w", "utf-8")
content = infile.read()
infile.close()
lines = content.split("\n")
lastLine = ""
result = []
#Filter deplicated lines
print("[info] Removing duplicated lines")
for line in lines:
    if (len(line) > 2 and line[-1] != " " and lastLine != line):
        result.append(line)
        lastChar = line
#Merge differential lines
        
print("[info] Merging differential lines")
lastChar = ""
twoPassResult = []
lastLine = ""
for line in result:
    if (lastChar == line[0:1]):
        #This line has the same starting char with the line above. But different char combinations
        tmp = lastLine.split(" ")[1]
        lineData = line.split(" ")[1]
        if (tmp != lineData):
            lastLine = line + tmp
    else:
        if (lastLine != ""):
            twoPassResult.append(lastLine)
        lastLine = line
        lastChar = line[0:1]
#Append the last line to the  2 pass result
twoPassResult.append(lastLine)
print("[info] Writing result to file.")
for line in twoPassResult:
    outfile.write(line)
    outfile.write("\n")
print("[DONE]")
outfile.close()
