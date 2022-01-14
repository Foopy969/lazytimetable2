from PyPDF2 import PdfFileReader
from json import dump

a,o=[],{}
f=open('MTT_2122S1_ASc2_V1.pdf', 'rb')
fr=PdfFileReader(f)

for i in range(fr.getNumPages()):
    t=fr.getPage(i).extractText().partition('Room\n')[2].partition('\nBuilding code' if i == 0 else '\nLast updated')[0].split('\n')
    a.extend(t)

    for j in range(len(a)):
        if a[j][0] == '(':
            a[j-1] += a[j]
            a[j] = ''
    a=[j for j in a if j != '']

for i in [a[i:i+7] for i in range(0,len(a),7)]:
    if i[0] not in o.keys():
        o[i[0]]=i[1:]
    o[i[0]].extend(i[1:])

with open('data.json', 'w+') as f:
    dump(o, f)
