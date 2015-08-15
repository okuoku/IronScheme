﻿#| License
Copyright (c) 2007-2015 Llewellyn Pritchard
All rights reserved.
This source code is subject to terms and conditions of the BSD License.
See docs/license.txt. |#

(library (ironscheme typed parsing)
  (export
    :
    ->
    parse-lambda-clause
    parse-type
    parse-arg-type
    parse-name-type-expr
    parse-return-type-body)
  (import 
    (ironscheme)
    (ironscheme syntax utils)
    (ironscheme typed parsing-helper))

  (define-syntax :
    (lambda (x)
      (define (parse type)
        (syntax-case type (->)
          [(arg ... -> ret)
            (with-syntax (((arg ...) (map parse-type #'(arg ...)))
                          (ret (parse-type #'ret)))
              #'((arg ...) ret))]
          [(type arg ...) 
            (with-syntax (((arg ...) (map parse-type #'(arg ...))))
              #'(type arg ...))]
          [type #'type]))
      (syntax-case x ()
        [(_ id type)
          (identifier? #'id)
          (with-syntax [(type-spec-id (syntax-format ":~a" #'id #'id))
                        (type (parse #'type))]
            #'(define-syntax type-spec-id
                (make-compile-time-value 'type)))])))    
                
  (define (parse-lambda-clause x)
    (syntax-case x (:)
      [((arg ...) b b* ...)
        (with-syntax ((((id type) ...) (map parse-arg-type #'(arg ...)))
                      ((ret-type b b* ...) (parse-return-type-body #'(b b* ...))))        
          (with-syntax (((type ...) (map parse-type #'(type ...)))
                        (ret-type (parse-type #'ret-type)))
            #'((id ...) 
               ((type ...) ret-type)
               b b* ...)))]))
     
  (define (parse-arg-type x)
    (syntax-case x (:)
      [(arg : type) 
        #'(arg type)]
      [arg 
        #'(arg Object)]))
      
  (define (parse-name-type-expr x)
    (syntax-case x (:)
      [(name : type expr) 
        #'(name type expr)]
      [(name expr) 
        #'(name Object expr)]))
      
  (define (parse-return-type-body x)
    (syntax-case x (:)
      [(: ret-type b b* ...) 
        #'(ret-type b b* ...)]
      [(b b* ...) 
        #'(Object b b* ...)])))
    