from PyPDF2 import PdfFileReader
from json import dump

a,o=[],{}
f=open('MTT_2122S1_ASc2_V1.pdf', 'rb')
fr=PdfFileReader(f)

for i in range(fr.getNumPages()):
    t=fr.getPage(i).extractText().partition('TimeRoom')[2].partition('\nBuilding code' if i == 0 else '\nLast updated')[0].split('\n')

    for j in range(len(t)):
        if len(t[j]) == 8:
            t[j] += t[j+1]
            t[j+1] = ''
    t=[j for j in t if j != '']

    for j in range(len(t)):
        if t[j][0].isdigit():
            t[j-1] += t[j]
            t[j] = ''
    t=[j for j in t if j != '']
        
    a.extend(t)

for i in a:
    s=[j[::-1] for j in i[::-1].split(' - ', 1)]
    if i[:8] not in o:
        o[i[:8]]=[s[1][12:-7]]
    o[i[:8]].extend([i[8:12], s[1][-6], s[1][-5:], s[0][5:]])

with open('data.json', 'w+') as f:
    dump(o, f)

