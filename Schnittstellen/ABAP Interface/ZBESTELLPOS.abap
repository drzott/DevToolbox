*&---------------------------------------------------------------------*
*& Report  Z_BESTELLPOS                                  *
*&                                                                     *
*&---------------------------------------------------------------------*
*&                                                                     *
*&                                                                     *
*&---------------------------------------------------------------------*

REPORT  Z_BESTELLPOS                           .
TYPES:
  BEGIN OF type_ekko,
    ekko TYPE ekko,
    ekpo TYPE ekpo,
    eket TYPE eket,
  END OF type_ekko.

DATA: a_wa    TYPE type_ekko,
      g_aedat TYPE ekko-aedat,
      path(500) VALUE '\\SAP_Bestellpositionen.txt',
*      path(500) VALUE '\\SAP_Bestellpositionen.txt',
      l_zeile TYPE string.

SELECT-OPTIONS: s_aedat FOR g_aedat.

INITIALIZATION.

  MOVE: 'I' TO s_aedat-sign,
  'EQ' TO s_aedat-option.

  s_aedat-low = sy-datum - 20.
  s_aedat-high = sy-datum.
  APPEND s_aedat.



START-OF-SELECTION.

END-OF-SELECTION.

  DATA: lt_lines TYPE TABLE OF tline,
        l_feld TYPE dfies-fieldname,
        a_lines TYPE tline.
  FIELD-SYMBOLS: <f> TYPE ANY.

  CALL FUNCTION 'READ_TEXT'
    EXPORTING
*   CLIENT                        = SY-MANDT
      id                            = 'ST'
      language                      = 'D'
      name                          = 'ZSBESTELLPOSITIONEN'
      object                        = 'TEXT'
*   ARCHIVE_HANDLE                = 0
*   LOCAL_CAT                     = ' '
* IMPORTING
*   HEADER                        =
    TABLES
      lines                         = lt_lines
* EXCEPTIONS
*   ID                            = 1
*   LANGUAGE                      = 2
*   NAME                          = 3
*   NOT_FOUND                     = 4
*   OBJECT                        = 5
*   REFERENCE_CHECK               = 6
*   WRONG_ACCESS_TO_ARCHIVE       = 7
*   OTHERS                        = 8
            .
  IF sy-subrc <> 0.
* MESSAGE ID SY-MSGID TYPE SY-MSGTY NUMBER SY-MSGNO
*         WITH SY-MSGV1 SY-MSGV2 SY-MSGV3 SY-MSGV4.
  ENDIF.

  OPEN DATASET path FOR OUTPUT IN BINARY MODE.
  LOOP AT lt_lines INTO a_lines.
    MOVE a_lines-tdline TO l_zeile.
    CONDENSE l_zeile NO-GAPS.
    TRANSFER l_zeile TO path.
    TRANSFER '~#~' TO path.
  ENDLOOP.
  TRANSFER cl_abap_char_utilities=>cr_lf TO path.
  SELECT * INTO a_wa FROM
  ( (  ekko AS a INNER JOIN ekpo AS b
    ON a~ebeln EQ b~ebeln )
   INNER JOIN eket AS c
    ON b~ebeln EQ c~ebeln AND
       b~ebelp EQ c~ebelp )
    WHERE
     ( b~aedat IN s_aedat OR c~bedat IN s_aedat ).


    LOOP AT lt_lines INTO a_lines.
      CONCATENATE 'a_wa-' a_lines-tdline INTO l_feld.
      ASSIGN (l_feld) TO <f>.
      CHECK sy-subrc IS INITIAL.
      MOVE <f> TO l_zeile.
      TRANSFER l_zeile TO path.
      TRANSFER '~#~' TO path.
    ENDLOOP.

    TRANSFER cl_abap_char_utilities=>cr_lf TO path.

*    WRITE: /01 a_wa-ekko-ebeln,
*            13 a_wa-ekpo-ebelp,
*            20 a_wa-eket-etenr,
*            25 a_wa-ekbe-belnr,
*            36 a_wa-makt-maktx,
*            85 a_wa-ekpo-aedat.
  ENDSELECT.

  CLOSE DATASET path.