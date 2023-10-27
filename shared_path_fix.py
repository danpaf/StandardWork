import re
import sys
from os import path

if len(sys.argv) != 4:
    print('Usage: python3 shared_path_fix.py target_sln target_csproj path_to_shared_csproj')
    exit(-1)

target_sln = sys.argv[1]
target_csproj = sys.argv[2]
shared = path.abspath(sys.argv[3])


csproj = open(target_csproj, 'r', encoding='UTF-8').read()
csproj = re.sub(r'Include=\".*StandardShared.*', f'Include="{shared}" />', csproj)

open(target_csproj, 'w', encoding='UTF-8').write(csproj)

sln = open(target_sln, 'r', encoding='UTF-8').read()
sln = re.sub(r', \".*StandardShared\.csproj\"', f', "{shared}"', sln)

open(target_sln, 'w', encoding='UTF-8').write(sln)