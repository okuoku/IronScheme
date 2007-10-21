(define let
  (macro (args . body)
    (if (symbol? args)
        ;; cant use let here yet :(
        ((lambda (name args body)
           `(let ((,name #f))
              (set! ,name (lambda ,(map first args) ,@body))
              (,name ,@(map second args))))
         args (car body) (cdr body))
        `((lambda ,(map first args) ,@body) ,@(map second args)))))

(define define-macro
  (macro (nargs . body)
    (let ((name (car nargs))
          (args (cdr nargs)))
      `(define ,name (macro ,args ,@body)))))

(define-macro (begin . e)
  `((lambda () ,@e)))

(define-macro (let* args . body)
  (if (null? (cdr args))
      `(let ,args ,@body)
      `(let (,(car args))
         (let* ,(cdr args) ,@body))))

(define-macro (and . e)
  (if (null? e) #t
      (if (null? (cdr e)) (car e)
          `(if ,(car e)
               (and ,@(cdr e))
               #f))))

(define-macro (or . e)
  (if (null? e) #f
      (if (null? (cdr e)) (car e)
          (let ((t (gensym)))
            `(let ((,t ,(car e)))
               (if ,t ,t
                   (or ,@(cdr e))))))))

;; test mother
(let ((b '(1 2 3)))
  (let* ((a b)
         (b (cons 0 a)))
    (let bar ((a b))
      (if (null? a)
          (begin
            (display "eureka!")
            (newline))
          (begin
            (display a)
            (newline)
            (bar (cdr a)))))))

(define tak
  (lambda (x y z)
    (if (not (< y x))
        z
        (tak (tak (- x 1) y z)
             (tak (- y 1) z x)
             (tak (- z 1) x y) ))))

(time
 (tak 18 12 6) )

(newline)
(display "Press any key to exit.")
(read-char)
