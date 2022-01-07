Feature: Coloring code using console interface
	A console program to color the source code written in chosen programming language

#@Test
#Scenario: User Wants To See Help Info
#	Given the user uses Console
#	When the user types -h
#	Then the console should display help command output

@Test
Scenario: User Types Command That Doesnt Exist
	Given the user uses Console
	When the user types -d
	Then throws exception with message "ArgumentException: Unknown argument: -d"
	And the console should not display any output

@Test
Scenario: User Doesnt Type Anything
	Given the user uses Console
	When the user types
	Then the console should not display any output

@Test
Scenario: User Forgets To Type In Output File
	Given the user uses Console
	When the user types -i ../../../DemoContent/mtl_texture_separator.py -f HTML -l Python -c DayTheme
	Then the console should not display any output

@Test
Scenario: User Forgets To Type in Language
	Given the user uses Console
	When the user types -i ../../../DemoContent/mtl_texture_separator.py -f HTML -c DayTheme ../../../DemoContent/demo2.html
	Then the file at ../../../DemoContent/demo2.html should not be created
	Then the console should not display any output

@Test
Scenario: User Forgets To Type in Output Format
	Given the user uses Console
	When the user types -i  ../../../DemoContent/mtl_texture_separator.py -l Python -c DayTheme ../../../DemoContent/demo3.html
	Then throws exception with message "NullReferenceException: Object reference not set to an instance of an object."
	Then the file at ../../../DemoContent/demo3.html should not be created
	And the console should not display any output

@Test
Scenario: User Forgets To Type in Theme
	Given the user uses Console
	When the user types -i ../../../DemoContent/mtl_texture_separator.py -f HTML -l Python -c DayTheme ../../../DemoContent/demo4.html
	Then DayTheme theme is used
	And the file at ../../../DemoContent/demo4.html should be created
	And the console should not display any output

@Test
Scenario: User Types Valid Commands And Chooses DayTheme Python HTML
	Given the user uses Console
	When the user types -i ../../../DemoContent/mtl_texture_separator.py -f HTML -l Python -c DayTheme ../../../DemoContent/demo.html
	Then Python language is used
	And DayTheme theme is used
	And output is in HTML format
	And the file at ../../../DemoContent/demo.html should be created
	And the console should not display any output

@Test
Scenario: User Types Valid Commands And Chooses DarkulaTheme
	Given the user uses Console
	When the user types -i ../../../DemoContent/mtl_texture_separator.py -f HTML -l Python -c DarkulaTheme ../../../DemoContent/demo.html
	Then DarkulaTheme theme is used
	And the console should not display any output

@Test
Scenario: User Types Valid Commands And Chooses OneMonokaiMockTheme
	Given the user uses Console
	When the user types -i ../../../DemoContent/mtl_texture_separator.py -f HTML -l Python -c OneMonokaiMockTheme ../../../DemoContent/demo.html
	Then OneMonokaiMockTheme theme is used
	And the console should not display any output