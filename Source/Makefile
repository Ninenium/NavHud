all: build install

build:
	xbuild navhud.csproj
clean:
	-rm -rf obj
	-rm -rf bin

#package: install
#	(cd Output; zip -r NavHud.zip ../*)

install:
	-mkdir -p ../GameData/NavHud/Plugins
	cp bin/Debug/NavHud.dll ../GameData/NavHud/Plugins/
#	cp ../Protractor.version ./Output/Protractor/
#	cp LICENSE ./Output/Protractor/
#	cp RPMDefs.cfg ./Output/Protractor/
#	#cp gpl.txt ./Output/Protractor/
#	#cp mit.txt ./Output/Protractor/

dist: build install clean
	-rm ~/NavHud.zip
	(cd ..; zip -r ~/NavHud.zip *)
