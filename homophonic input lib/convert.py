#Required csv input from online json to csv converter
import codecs
infile = open ('converted.csv',encoding="utf-8")
outfile = codecs.open("output.dll", "w", "utf-8")
content = infile.read()
word = content.split(",")
infile.close()
for char in word:
    chineseChar = char.split("/")[0]
    pron = char.split("/")[1]
    #pronShorten = pron[0:-1] #移除聲調
    pronShorten = pron #不移除聲調
    outfile.write(chineseChar)
    outfile.write(" ")
    print("[info] Matching for: " + pronShorten)
    matchList = []
    for match in word:
        #Check if any word have same pron
        matchChar = match.split("/")[0]
        matchPron = match.split("/")[1]
        #matchPronShorten = matchPron[0:-1] #移除聲調
        matchPronShorten = matchPron #不移除聲調
        if (pronShorten == matchPronShorten and chineseChar != matchChar and matchChar not in matchList ):
            #如果兩個字的讀音不計聲調一樣
            outfile.write(matchChar)
            matchList.append(matchChar)
    outfile.write("\n")
outfile.close()
