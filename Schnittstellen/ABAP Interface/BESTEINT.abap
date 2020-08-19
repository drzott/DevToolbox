*&K--------------------------------------------------------------------*
*&K Report  Z_SIM_BESTEINT
*&K--------------------------------------------------------------------*
*&K                                                                    *
*&K  Autor     : Andreas Schwarz (AS)                                  *
*&K  Erstellt  : 01/2018                                               *
*&K  Logbuch (Änderungen):  wer, wann, warum, was geändert?            *
*&K--------------------------------------------------------------------*

REPORT z_sim_besteint.


DATA: s_dat     TYPE ekko-aedat,
      file(100) VALUE 'SAP_BestEint.txt',
      path(500) VALUE '',
      showerg   TYPE i,
      showend   TYPE i.

showerg = 0.
showend = 0.

SELECTION-SCREEN BEGIN OF BLOCK bzeitraum WITH FRAME TITLE tizeit.

SELECTION-SCREEN BEGIN OF LINE.
PARAMETERS: p_rbtage RADIOBUTTON GROUP gzer DEFAULT 'X'.
SELECTION-SCREEN COMMENT (45) lbltage FOR FIELD p_rbtage.
PARAMETERS: i_tage TYPE i DEFAULT 20.
SELECTION-SCREEN END OF LINE.

SELECTION-SCREEN BEGIN OF LINE.
PARAMETERS: p_rbdat RADIOBUTTON GROUP gzer.
SELECTION-SCREEN COMMENT (42) lbldatum FOR FIELD p_rbdat.
SELECT-OPTIONS: s_aedat FOR s_dat.
SELECTION-SCREEN END OF LINE.

SELECTION-SCREEN END OF BLOCK bzeitraum.


INITIALIZATION.

  MOVE: 'I' TO s_aedat-sign,
  'EQ' TO s_aedat-option.

  s_aedat-low = sy-datum - 30.
  s_aedat-high = sy-datum.
  APPEND s_aedat.
  tizeit = 'Auswahl Zeiteingrenzung'.
  lbltage = 'Anzahl Tage (täglicher automatischer Export)'.
  lbldatum = 'Datum von bis (manueller Export)'.


START-OF-SELECTION.

END-OF-SELECTION.

  CASE sy-sysid.
    WHEN 'AT2'.
      CONCATENATE '\\xxx\sapmnt\trans\data\' sy-sysid '_' file INTO path.
    WHEN 'AQ2'.
      CONCATENATE '\\xxx\sapmnt\trans\data\' sy-sysid '_' file INTO path.
    WHEN 'AP2'.
      CONCATENATE '\\xxx\sapmnt\trans\data\' file INTO path.
  ENDCASE.


  IF p_rbtage = 'X'.
    REFRESH s_aedat.
    s_aedat-low = sy-datum - i_tage.
    s_aedat-high = sy-datum.
    APPEND s_aedat.

  ENDIF.

  TYPES:
    BEGIN OF type_besteint,
      ekko TYPE ekko,
      ekpo TYPE ekpo,
      eket TYPE eket,
    END OF type_besteint.


  DATA: a_wa     TYPE type_besteint,
        lt_lines TYPE TABLE OF tline,
        l_feld   TYPE dfies-fieldname,
        a_lines  TYPE tline,
        l_zeile  TYPE string.
  FIELD-SYMBOLS: <f> TYPE any.

  CALL FUNCTION 'READ_TEXT'
    EXPORTING
      id       = 'ST'
      language = 'D'
      name     = 'ZBESTEINT'
      object   = 'TEXT'
    TABLES
      lines    = lt_lines.

*Datei für Ausgabe erstellen bzw öffnen.
  OPEN DATASET path FOR OUTPUT IN BINARY MODE.
*Spaltenüberschriften exportieren
  LOOP AT lt_lines INTO a_lines.
    MOVE a_lines-tdline TO l_zeile.
    CONDENSE l_zeile NO-GAPS.
    TRANSFER l_zeile TO path.
    TRANSFER '~#~' TO path.
    IF showerg = 1.
      WRITE l_zeile.
      WRITE '~#~'.
    ENDIF.
  ENDLOOP.
  TRANSFER cl_abap_char_utilities=>cr_lf TO path.
  IF showerg = 1.
    WRITE cl_abap_char_utilities=>cr_lf.
  ENDIF.

*Daten sammeln und in Datei schreiben
  SELECT * INTO a_wa
  FROM ekko AS a
    INNER JOIN ekpo AS b
      ON a~ebeln EQ b~ebeln

    INNER JOIN eket AS c
      ON b~ebeln EQ c~ebeln
        AND b~ebelp EQ c~ebelp

  WHERE a~aedat IN s_aedat
    OR a~bedat IN s_aedat
    OR b~aedat IN s_aedat
    OR c~bedat IN s_aedat
    OR a~ebeln IN ( SELECT x~ebeln FROM ekbe AS x WHERE x~budat IN s_aedat OR x~cpudt IN s_aedat OR x~bldat IN s_aedat )
  .

    LOOP AT lt_lines INTO a_lines.
      CONCATENATE 'a_wa-' a_lines-tdline INTO l_feld.
      ASSIGN (l_feld) TO <f>.
      CHECK sy-subrc IS INITIAL.
      MOVE <f> TO l_zeile.
      TRANSFER l_zeile TO path.
      TRANSFER '~#~' TO path.
      IF showerg = 1.
        WRITE l_zeile.
        WRITE '~#~'.
      ENDIF.
    ENDLOOP.

    TRANSFER cl_abap_char_utilities=>cr_lf TO path.
    IF showerg = 1.
      WRITE cl_abap_char_utilities=>cr_lf.
    ENDIF.

  ENDSELECT.

*Exportdatei schließen
  CLOSE DATASET path.


  IF showend = 1.

    IF p_rbtage = 'X'.

      WRITE 'Automatisch    :   '.
      WRITE i_tage.
      WRITE s_aedat-low.
      WRITE s_aedat-high.

    ENDIF.

    IF p_rbdat = 'X'.

      WRITE 'MANUELL   :   '.
      WRITE s_aedat-low.
      WRITE s_aedat-high.

    ENDIF.
    WRITE 'OK'.
  ENDIF.