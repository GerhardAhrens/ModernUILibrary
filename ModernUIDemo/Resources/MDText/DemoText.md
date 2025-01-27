##ReleaseNote zu **ReleaseNoteViewDemo**
***

Version 1.0 from 10.7.2018
* * *

*This text will be italic*
_This will also be italic_

**This text will be bold**
__This will also be bold__

***
Header 1
========

Lorem ipsum dolor sit amet, consectetur adipiscing elit. Duis orci erat, 
hendrerit et interdum at, feugiat non diam. Vestibulum id odio non elit 
consectetur faucibus quis a urna. 

Header 2
-----------

Lorem ipsum dolor sit amet, consectetur adipiscing elit. Duis orci erat, 
hendrerit et interdum at, feugiat non diam. Vestibulum id odio non elit 
consectetur faucibus quis a urna. 

# Header Level 1

Lorem ipsum dolor sit amet, consectetur adipiscing elit. Duis orci erat, 
hendrerit et interdum at, feugiat non diam. Vestibulum id odio non elit 
consectetur faucibus quis a urna. 

## Header Level 2

Lorem ipsum dolor sit amet, consectetur adipiscing elit. Duis orci erat, 
hendrerit et interdum at, feugiat non diam. Vestibulum id odio non elit 
consectetur faucibus quis a urna. 

### Header Level 3

Lorem ipsum dolor sit amet, consectetur adipiscing elit. Duis orci erat, 
hendrerit et interdum at, feugiat non diam. Vestibulum id odio non elit 
consectetur faucibus quis a urna. 

#### Header Level 4

Lorem ipsum dolor sit amet, consectetur adipiscing elit. Duis orci erat, 
hendrerit et interdum at, feugiat non diam. Vestibulum id odio non elit 
consectetur faucibus quis a urna. 
 ***

Dashes:

---

 ---
 
  ---

   ---

    ---

 Dashes StrokeDashArray:
- - -

 - - -
 
  - - -

   - - -

    - - -


Asterisks:

***

 ***
 
  ***

   ***

    ***

Asterisks StrokeDashArray:
* * *

 * * *
 
  * * *

   * * *

    * * *


Underscores:

___

 ___
 
  ___

   ___

    ___

Underscores StrokeDashArray:
_ _ _

 _ _ _
 
  _ _ _

   _ _ _

    _ _ _

***
![image1](http://placehold.it/350x150)

![imageleft](http://placehold.it/100x150/0000FF)![imageright](http://placehold.it/100x150/00FFFF)

* Local images

![localimage](sampleimage.jpg)
***

Just a [URL](/url/).

[URL and title](/url/ "title").

[URL and title](/url/  "title preceded by two spaces").

[URL and title](/url/	"title preceded by a tab").

[URL and title](/url/ "title has spaces afterward"  ).

[URL wrapped in angle brackets](</url/>).

[URL w/ angle brackets + title](</url/> "Here's the title").

[Empty]().

[https://stackoverflow.com](https://stackoverflow.com)

[With parens in the URL](http://en.wikipedia.org/wiki/WIMP_(computing))

(With outer parens and [parens in url](/foo(bar)))


[With parens in the URL](/foo(bar) "and a title")

(With outer parens and [parens in url](/foo(bar) "and a title")) 

***
First we have a simple numeric list

1. Alpha
1. Beta
2. Gamma
3. Delta

Then we have a simple unordered list

* Han
* Leia
* Luke
* Obiwan

## Unordered

Asterisks tight:

*	asterisk 1
*	asterisk 2
*	asterisk 3


Asterisks loose:

*	asterisk 1

*	asterisk 2

*	asterisk 3

* * *

Pluses tight:

+	Plus 1
+	Plus 2
+	Plus 3


Pluses loose:

+	Plus 1

+	Plus 2

+	Plus 3

* * *


Minuses tight:

-	Minus 1
-	Minus 2
-	Minus 3


Minuses loose:

-	Minus 1

-	Minus 2

-	Minus 3


## Ordered

Tight:

1.	First
2.	Second
3.	Third

and:

1. One
2. Two
3. Three


Loose using tabs:

1.	First

2.	Second

3.	Third

and using spaces:

1. One

2. Two

3. Three

Multiple paragraphs:

1.	Item 1, graf one.

    Item 2. graf two. The quick brown fox jumped over the lazy dog's
    back.
    
2.	Item 2.

3.	Item 3.



## Nested

*	Tab
    *	Tab
        *	Tab

Here's another:

1. First
2. Second:
    * Fee
    * Fie
    * Foe
3. Third

Same thing but with paragraphs:

1. First

2. Second:
    * Fee
    * Fie
    * Foe

3. Third


This was an error in Markdown 1.0.1:

*	this

    *	sub

    that

## Tabelle

| Left-Aligned  | Center Aligned  | Right Aligned |
|:------------- |:---------------:| -------------:|
| Row 1         | Cell 2          | Cell 3        |
| Row 2         | Cell 5          | Cell 6        |
| Row 3         | Cell 8          | Cell 9        |