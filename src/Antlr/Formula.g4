grammar Formula;

options 
{
	language=CSharp;
}

expression
   : multiplyingExpression ((PLUS | MINUS) multiplyingExpression)*
   ;

multiplyingExpression
   : negateExpression ((TIMES | DIV) negateExpression)*
   ;


negateExpression
   : MINUS? powExpression   
   ;


powExpression
   : atom (POW atom)?
   ;


atom
   : number
   | parameter
   | LPAREN expression RPAREN
   | func
   | literal
   ;


func
   : funcname LPAREN expression(',' expression)* RPAREN
   ;


funcname
   : variable     
   ;


parameter
   : LBRACKET DIGIT (COMMA variable)? RBRACKET
   ;


number
   : MINUS? DIGIT + (POINT DIGIT +)?
   ;


variable
   : LETTER (LETTER | DIGIT)*
   ;   
      

literal
   : STRING
   ;   


LPAREN
   : '('
   ;


RPAREN
   : ')'
   ;

LBRACKET
   : '{'
   ;


RBRACKET
   : '}'
   ;


PLUS
   : '+'
   ;


MINUS
   : '-'
   ;


TIMES
   : '*'
   ;


DIV
   : '/'
   ;


POINT
   : '.'
   ;


POW
   : '^'
   ;
   

STRING
   : SQUOTE (ESC | ~ [\'\\])* SQUOTE
   ;

ESC
   : '\\' ([\'\\/bfnrt])
   ;


LETTER
   : ('a' .. 'z') | ('A' .. 'Z')
   ;
   

DIGIT
   : ('0' .. '9')
   ;


COMMA
   : ','
   ;


SQUOTE
   : '\''
   ;


WS
   : [ \r\n\t] + -> channel (HIDDEN)
   ;