/**
 * Transform class to help async validator rule
 */
export default class Transform {
  /**
   * to data
   * @param value
   * @returns {*}
   */
  static toDate (value) {
    if (value) {
      return new Date(value)
    }
    return null
  }

  /**
   * to int
   * @param value
   * @returns {*}
   */
  static toInteger (value) {
    if (!Number.isNaN(value)) {
      return parseInt(value)
    }
    return null
  }

  /**
   * simple check the value is url
   * @param value
   * @returns {*|boolean}
   */
  static isSimpleUrl (value) {
    return value && (value.indexOf('http://') > -1 || value.indexOf('https://') > -1)
  }
}
