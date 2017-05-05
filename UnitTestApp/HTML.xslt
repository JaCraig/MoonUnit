<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:msxsl="urn:schemas-microsoft-com:xslt" exclude-result-prefixes="msxsl"
>
    <xsl:output method="html" indent="yes"/>

    <xsl:template match="/">
        <html>
            <body>
                <table>
                    <tr>
                        <th>Class</th>
                        <th>Method</th>
                        <th>Result</th>
                    </tr>
                    <xsl:for-each select="MoonUnit/Tests/Test">
                        <tr>
                            <td>
                                <xsl:for-each select="Class">
                                    <xsl:value-of select="@name"/>
                                </xsl:for-each>
                            </td>
                            <td>
                                <xsl:for-each select="Method">
                                    <xsl:value-of select="@name"/>
                                </xsl:for-each>
                            </td>
                            <td>
                                <xsl:for-each select="Passed">
                                    Passed
                                </xsl:for-each>
                                <xsl:for-each select="Failed">
                                    Failed<br />
                                    Expected: <xsl:value-of select="Expected"/><br />
                                    Result: <xsl:value-of select="Result"/><br />
                                    Error Message: <xsl:value-of select="ErrorText"/><br />
                                    Stack Trace: <xsl:value-of select="Trace"/><br />
                                    Error Type: <xsl:value-of select="ErrorType"/><br />
                                </xsl:for-each>
                            </td>
                        </tr>
                    </xsl:for-each>
                </table>
            </body>
        </html>
    </xsl:template>
</xsl:stylesheet>
